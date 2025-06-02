using System.Collections.Generic;
using Core.QFrameWork;
using UnityEngine;

namespace LevelSystem
{
    public class LevelMapController : BaseController
    {
        public Transform Player1StartPositions;
        public Transform Player2StartPositions;

        public Transform BoundCameraLeftDown;
        public Transform BoundCameraRightUp;
    }
}