using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlane : MonoBehaviour
    {
        /*
         * 05.01.21
         * add a link to the ARPlaneManager
         */
        private ARPlaneManager m_ARPlaneManager;
        [Tooltip("TAG for AR Plane.")]
        public string m_planeCloneTag = "planeClone";
        [Tooltip("UI Component to inform user about tapping on screen")]
        [SerializeField] private GameObject UIComponent;

        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_ARPlaneManager = GetComponent<ARPlaneManager>(); //new as of 05.01.21
            UIComponent.SetActive(true); //1.2.24
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    /*
                     * 05.03.21
                     * after object is spawned, disable the raycastManager 
                     * disable the plane manager
                     * remove any UI components
                     * then disable this script
                     */
                        m_RaycastManager.enabled = false;
                        m_ARPlaneManager.enabled = false;
                        GameObject[] PlaneClones = GameObject.FindGameObjectsWithTag(m_planeCloneTag);
                        for (int i = 0; i < PlaneClones.Length; i++)
                        {
                            Destroy(PlaneClones[i]);
                        }
                    //Destroy(GameObject.FindGameObjectWithTag(m_planeCloneTag));
                    //m_RaycastManager.enabled = false;
                    //remove UI components
                    UIComponent.SetActive(false); //1.2.24
                    //disable this script
                    enabled = false;
                    }
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;
                    }
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }
