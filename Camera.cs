using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Camera
    {
        Vector3 camTarget;
        Vector3 camPosition;
        Vector3 camPositionState;
        public Vector3 camTracker;
        public Vector3 camOffset;

        float cosAngle = 0;
        float tanAngle = 0;
        float sinAngle = 0;

        public Vector3 nextpos;
        float camSlow = 20f;

        public Camera()
        {
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 30f,30f);
            camPositionState = camPosition;
            CalculateAngles();
        }
        public Camera(Vector3 camTarget, Vector3 camPosition, Vector3 camPositionStat)
        {
            this.camTarget = camTarget;
            this.camPosition = camPosition;
            this.camPositionState = camPositionStat;

            CalculateAngles();
        }

        public Vector3 CamTarget
        {
            get { return camTarget; }
            set { camTarget = value; }
        }

        public Vector3 CamPosition
        {
            get { return camPosition; }
            set { camPosition = value; }
        }

        public Vector3 CamPositionState
        {
            get { return camPositionState; }
            set { camPositionState = value; }
        }

        public float CosAngle
        {
            get { return cosAngle; }
            set { cosAngle = value; }
        }

        public float SinAngle
        {
            get { return sinAngle; }
            set { sinAngle = value; }
        }

        public float TanAngle
        {
            get { return tanAngle; }
            set { tanAngle = value; }
        }

        private void CalculateAngles()
        {
            cosAngle = camPosition.X / (float)Math.Sqrt(Math.Pow(camPosition.X, 2) + Math.Pow(camPosition.Z, 2));
            sinAngle = camPosition.Z / (float)Math.Sqrt(Math.Pow(camPosition.X, 2) + Math.Pow(camPosition.Z, 2));
            tanAngle = camPosition.Z / camPosition.X;
        }

        public void Update()
        {
            // camOffset = Vector3.Lerp(camPosition,nextpos*1.1f,camSlow);
            // camTracker += Vector3.Normalize(Vector3.Lerp(camPosition, camOffset, 5f));
            camOffset = Vector3.Lerp(camPosition, nextpos, camSlow);
            camTracker += Vector3.Normalize(Vector3.Lerp(nextpos, camOffset, 20f));
        }

        public void Update1(Vector3 player)
        {
            camTracker = player;
        }
    }
}
