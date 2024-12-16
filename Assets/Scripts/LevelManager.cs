using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static CinemachineConfiner2D currentConfiner;

        private CinemachineBrain brain;
        private CinemachineCamera cam;

        static BoxCollider2D currentSection;

        // Use this for initialization
        void Start()
        {
            brain = CinemachineBrain.GetActiveBrain(0);
            currentConfiner = GameObject.Find("CM").GetComponent<CinemachineConfiner2D>();
        }

        public static void ChangeSection(string sectionName)
        {
            // Procure pelo objeto que contem o nome (sectionName)
            // E pega o colisor dele para ser o novo confiner 2D
            currentSection = GameObject.Find(sectionName).GetComponent<BoxCollider2D>();

            // Se o objeto for encontrado e tiver o colisor
            if (currentSection)
            {
                // Faz com que a camera limpe o cache do confiner anterior, esquece o confiner anterior
                currentConfiner.InvalidateBoundingShapeCache();

                // Define o novo confiner da camera
                currentConfiner.BoundingShape2D = currentSection;

                // Reposicionar o Right Limiter, alinhado max x do confiner (direita do confiner)
                GameObject rigthLimiter = GameObject.Find("Right");

                // Vector3(x, y, z)
                rigthLimiter.transform.position = new Vector3(
                    currentConfiner.BoundingShape2D.bounds.max.x,
                    rigthLimiter.transform.position.y
                    );

            }

        }
    }
}