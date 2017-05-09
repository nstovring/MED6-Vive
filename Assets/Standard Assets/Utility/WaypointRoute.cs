using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityStandardAssets.Utility
{
    public class WaypointsRoute : MonoBehaviour
    {
        public WaypointList waypointList = new WaypointList();
        private bool smoothRoute = true;
        private int numPoints;
        private Vector3[] points;
        private float[] distances;

        public WaypointsRoute(float length)
        {
            this.Length = length;
        }
        public float Length { get; private set; }

        public Transform[] Waypoints
        {
            get { return waypointList.items.ToArray(); }
        }

        //this being here will save GC allocs
        private int p0n;
        private int p1n;
        private int p2n;
        private int p3n;

        private float i;
        private Vector3 P0;
        private Vector3 P1;
        private Vector3 P2;
        private Vector3 P3;

        [Serializable]
        public class WaypointList
        {
            public WaypointsRoute track;
            public List<Transform> items = new List<Transform>();
        }

        public struct RoutePoint
        {
            public Vector3 position;
            public Vector3 direction;


            public RoutePoint(Vector3 position, Vector3 direction)
            {
                this.position = position;
                this.direction = direction;
            }
        }

        // Use this for initialization
        private void Awake()
        {
            if (Waypoints.Length > 1)
            {
                CachePositionsAndDistances();
            }
            smoothRoute = true;
            numPoints = Waypoints.Length;
        }


        public void beginPath(Vector3 target, GameObject parent)
        {
            // add waypoint
            Debug.Log("target" + target);
            GameObject waypoint = new GameObject("Waypoint " + (Waypoints.Length).ToString("000"));

            //IconManager.SetIcon(waypoint, IconManager.LabelIcon.Red);
            waypoint.transform.parent = parent.transform;

            // Debug.Log("pos" + waypoint.transform.position);
            // Debug.Log("local" + waypoint.transform.localPosition);

            //waypoint.transform.position = parent.transform.position + waypoint.transform.localPosition;
            waypoint.transform.localPosition = target + waypoint.transform.localPosition;

            // Debug.Log("pos" + waypoint.transform.position);
            // Debug.Log("local" + waypoint.transform.localPosition);

            //FeedController controller = new FeedController();
            int waypoints = Waypoints.Length;

            waypointList.items.Add(waypoint.transform);

            CachePositionsAndDistances();
            numPoints = Waypoints.Length;
        }

        public RoutePoint GetRoutePoint(float dist)
        {
            // position and direction
            Vector3 p1 = GetRoutePosition(dist);
            Vector3 p2 = GetRoutePosition(dist + 0.1f);
            Vector3 delta = p2 - p1;
            return new RoutePoint(p1, delta.normalized);
        }

        public Vector3 GetRoutePosition(float dist)
        {
            int point = 0;

            if (Length == 0)
            {
                Length = distances[distances.Length - 1];
            }
            // else if (dist > Length) {
            //     dist = Length;
            // }
            dist = Mathf.PingPong(dist, Length);
            //dist = Mathf.Repeat(dist, Length);

            while (distances[point] < dist)
            {
                point++;
            }

            // get nearest two points, ensuring points NOT wrap-around start & end of circuit
            p1n = (point - 1) >= 0 ? point - 1 : 0;
            p2n = point;

            // found point numbers, now find interpolation value between the two middle points
            i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

            if (smoothRoute)
            {
                // smooth catmull-rom calculation between the two relevant points
                // get indices for the surrounding 2 points, because
                // four points are required by the catmull-rom function
                // p0n = ((point - 2) + numPoints) % numPoints;
                // p3n = (point + 1) % numPoints;
                p0n = point - 2 >= 0 ? point - 2 : 0;
                p3n = point + 1 < points.Length ? point + 1 : point;

                P0 = points[p0n];
                P1 = points[p1n];
                P2 = points[p2n];
                P3 = points[p3n];

                return CatmullRom(P0, P1, P2, P3, i);
            }
            else
            {
                // simple linear lerp between the two points:
                p1n = point - 1 >= 0 ? point - 1 : point;
                p2n = point;

                return Vector3.Lerp(points[p1n], points[p2n], i);
            }
        }


        private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
        {
            // comments are no use here... it's the catmull-rom equation.
            // Un-magic this, lord vector!
            return 0.5f *
                   ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                    (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
        }


        private void CachePositionsAndDistances()
        {
            // transfer the position of each point and distances between points to arrays for
            // speed of lookup at runtime
            // Here we make points and their accumulate distances
            points = new Vector3[Waypoints.Length];
            distances = new float[Waypoints.Length];

            float accumulateDistance = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                var t1 = Waypoints[i % Waypoints.Length];
                var t2 = Waypoints[(i + 1) % Waypoints.Length];
                if (t1 != null && t2 != null)
                {
                    Vector3 p1 = t1.position;
                    Vector3 p2 = t2.position;
                    points[i] = Waypoints[i % Waypoints.Length].position;
                    distances[i] = accumulateDistance;
                    accumulateDistance += (p1 - p2).magnitude;
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        private void DrawGizmos(bool selected)
        {
            waypointList.track = this;
            if (Waypoints.Length > 1)
            {
                numPoints = Waypoints.Length;

                CachePositionsAndDistances();
                // Get full Length of way from last object from distances
                Length = distances[distances.Length - 1];

                Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);
                Vector3 prev = Waypoints[0].position;

                if (smoothRoute)
                {
                    float editorVisualisationSubsteps = 100;
                    for (float dist = 0; dist < Length - 1; dist += Length / editorVisualisationSubsteps)
                    {
                        Vector3 next = GetRoutePosition(dist + 1);
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                }
                else
                {
                    for (int n = 0; n < Waypoints.Length - 1; ++n)
                    {
                        Vector3 next = Waypoints[(n + 1)].position;
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                }
            }
        }
    }
}
