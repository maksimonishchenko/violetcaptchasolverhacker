using System;
using UnityEngine;

namespace Game
{
    public class Cell:  IDisposable
    {
        private int mX;
        private int mY;
        private int mNum;
        private Color mColor;
        private GameObject mGameObject;
        private QuadVC mQuadVc;
        private float mScale;
        private float mOffset;
        private Vector3 mMinXYCenter;

        public Action<Cell> Select;
        public Action<Cell> Drag;
        public Action<Cell> Drop;
        
        public Cell(int x, int y, int num, Color color, GameObject cellPrefab, Vector3 minXYCenter, Canvas canvas, float scale, float offset)
        {
            mMinXYCenter = minXYCenter;
            mScale = scale;
            mOffset = offset;
            mX = x;
            mY = y;
            mNum = num;
            mColor = color;

            mGameObject = GameObject.Instantiate(cellPrefab, GetPos(mMinXYCenter, mOffset, mScale), Quaternion.Euler(270f,0f,0f));
            mGameObject.transform.localScale = Vector3.one * scale * 0.1f;  //Plane в 10 раз длиннее 1 юнита в unity
            mGameObject.GetComponentInChildren<TextMesh>().fontSize = Mathf.RoundToInt(scale * 0.5f);
            mGameObject.GetComponentInChildren<TextMesh>().text = num.ToString();
            mGameObject.GetComponentInChildren<TextMesh>().offsetZ = -offset;
            mGameObject.GetComponent<MeshRenderer>().material.color = color;
            mQuadVc = mGameObject.GetComponent<QuadVC>();
            mQuadVc.SetCamera(canvas.worldCamera);

            mQuadVc.Select += OnVCSelect;
            mQuadVc.Drag += OnVCDrag;
            mQuadVc.Drop += OnVCDrop;
        }

        private Vector3 GetPos(Vector3 minXYCenter, float offset, float scale)
        {
            return minXYCenter + LocalPos() + Vector3.back * offset * scale;
        }
        
        public void RenderBorder(Material lineMaterial)
        {
            GL.PushMatrix();
            GL.Color(Color.white);
            var pos = mMinXYCenter + LocalPos();
            
            var p1 = pos - Vector3.right * mScale * 0.5f - Vector3.up * mScale * 0.5f;
            var p2 = pos - Vector3.right * mScale * 0.5f + Vector3.up * mScale * 0.5f;
            var p3 = pos + Vector3.right * mScale * 0.5f - Vector3.up * mScale * 0.5f;
            var p4 = pos + Vector3.right * mScale * 0.5f + Vector3.up * mScale * 0.5f;
            
            
            lineMaterial.SetPass(0);
            
            GL.Begin(GL.LINES);
            GL.Vertex3(p1.x,p1.y,p1.z);
            GL.Vertex3(p2.x,p2.y,p2.z);
            
            GL.Vertex3(p2.x,p2.y,p2.z);
            GL.Vertex3(p4.x,p4.y,p4.z);
            
            GL.Vertex3(p4.x,p4.y,p4.z);
            GL.Vertex3(p3.x,p3.y,p3.z);
            
            GL.Vertex3(p3.x,p3.y,p3.z);
            GL.Vertex3(p1.x,p1.y,p1.z);
            
            GL.End();
            GL.PopMatrix();
        }

        public bool Check()
        {
            var realPos = GetPos(mMinXYCenter, mOffset, mScale);
            var realPos2D = new Vector2(realPos.x, realPos.y);
            
            var curPos =  mGameObject.transform.position;
            var curPos2D = new Vector2(curPos.x, curPos.y);

            mGameObject.transform.position = realPos;
            
            if (Vector2.Distance(realPos2D,curPos2D) > 0.5f * mScale)
            {
                mGameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                return false;
            }
            
            mGameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            return true;
        }
        
        public void DropToStack(Vector3 stackBottom, float stackH, float stackW)
        {
            var stackCenter = stackBottom + Vector3.up * stackH * 0.5f + Vector3.left * 0.5f * stackW;

            var randomX = UnityEngine.Random.Range(-0.5f * stackW, 0.5f * stackW);
            var randomY = UnityEngine.Random.Range(-0.5f * stackH, 0.5f * stackH);

            mGameObject.transform.position = stackCenter + randomX * Vector3.right + randomY * Vector3.up;
        }
        
        void OnVCSelect()
        {
            Select?.Invoke(this);
        }
        
        void OnVCDrag()
        {
            Drag?.Invoke(this);
        }
        
        void OnVCDrop()
        {
            Drop?.Invoke(this);
        }
        
        public Vector3 LocalPos()
        {
            return new Vector3(mX * mScale, mY * mScale, 0f);
        }

        public void Dispose()
        {
            mQuadVc.Select -= OnVCSelect;
            mQuadVc.Drag -= OnVCDrag;
            mQuadVc.Drop -= OnVCDrop;
            
            #if UNITY_EDITOR
                GameObject.DestroyImmediate(mGameObject);
            #else
                GameObject.Destroy(mGameObject);
            #endif
        }
    }
  
}
