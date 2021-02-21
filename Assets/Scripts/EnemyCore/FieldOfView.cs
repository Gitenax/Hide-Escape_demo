using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCore
{
    /// <summary>
    /// Поле зрение объекта
    /// </summary>
    public class FieldOfView : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private float _viewRadius;                       // Радиус от центра объекта
        [Range(0, 360), SerializeField] private float _fovAngle;          // Угол обзора

        [SerializeField] private LayerMask _targetMask;                   // Маска для искомой цели
        [SerializeField] private LayerMask _obstacleMask;                 // Маска препятствий
        
        // Отрисовка меша зоны обзора
        [SerializeField] private float _meshResolution; 
        [SerializeField] private MeshFilter _meshFilter;
#pragma warning restore CS0649
        
        private List<Transform> _visibleTargets = new List<Transform>();  // Список целей попавших в обзор
        private Mesh _viewMesh;
        
        
        public event Action PlayerDetected;

        
        public List<Transform> VisibleTargets => _visibleTargets;

        public float ViewRadius => _viewRadius;

        public float FOVAngle => _fovAngle;
        
        
        // Возвращает вектор направления из угла
        public Vector3 DirFromAngle(float angle, bool global)
        {
            if (global == false)
                angle += transform.localEulerAngles.y;
            
            return new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0,
                Mathf.Cos(angle * Mathf.Deg2Rad));
        }


        private void Start()
        {
            _viewMesh = new Mesh();
            _viewMesh.name = "View mesh";
            _meshFilter.mesh = _viewMesh;
            
            // Старт сканирования окружения
            StartCoroutine(nameof(ScanEnvironment), 0.2f);
        }

        private void LateUpdate()
        {
            // Отрисовка меша зоны видимости в игре
            DrawFieldOfView();
        }

        // Поиск целей
        private void FindVisibleTargets()
        {
            _visibleTargets.Clear();
            Collider[] targetsInBound = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

            foreach (var target in targetsInBound)
            {
                var dirToTarget = (target.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < _fovAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                    {
                        _visibleTargets.Add(target.transform);
                        PlayerDetected?.Invoke();
                    }
                }
            }
        }

        private IEnumerator ScanEnvironment(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void DrawFieldOfView()
        {
            int steps = Mathf.RoundToInt(_fovAngle * _meshResolution);
            float stepAngle = _fovAngle / steps;
            List<Vector3> castPoints = new List<Vector3>();

            for (int i = 0; i <= steps; i++)
            {
                float angle = transform.eulerAngles.y - _fovAngle / 2 + stepAngle * i;
                ViewcastInfo viewcast = ViewCast(angle);
                castPoints.Add(viewcast.Point);
            }

            int vertexCount = castPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];
            
            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(castPoints[i]);
                
                if(i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            
            // Генерация меша на основе вершин и треугольников
            _viewMesh.Clear();
            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
        }

        // Получение информации от брошенного луча
        private ViewcastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleMask))
                return new ViewcastInfo(true, hit.point, hit.distance, globalAngle);
            
            return new ViewcastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
        }
        
        // Структура возвращающая данные рейкаста для отсечения лишних данных
        public struct ViewcastInfo
        {
            public bool Hit;
            public Vector3 Point;
            public float Distance;
            public float Angle;

            public ViewcastInfo(bool hit, Vector3 point, float distance, float angle)
            {
                Hit = hit;
                Point = point;
                Distance = distance;
                Angle = angle;
            }
        }
    }
}
