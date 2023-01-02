using System;
using UnityEngine;

namespace Game
{
    public class QuadVC : MonoBehaviour
    {
        private Camera mCamera;

        private Vector3 mOffset;
        private float mZCoord;

        public event Action Select;
        public event Action Drag;
        public event Action Drop;
        
        public void SetCamera(Camera camera)
        {
            mCamera = camera;
        }
        

        void OnMouseDown()
        {
            mZCoord = mCamera.WorldToScreenPoint(transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
            Select?.Invoke();
        }
        
        void OnMouseDrag()
        {
            Drag?.Invoke();
            transform.position = GetMouseWorldPos() + mOffset;
        }

        private void OnMouseUp()
        {
            Drop?.Invoke();
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mZCoord;
            return mCamera.ScreenToWorldPoint(mousePoint);
        }
    }
}


