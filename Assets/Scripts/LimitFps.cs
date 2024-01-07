using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class LimitFps : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 1;  // 1 = 60 fps, 2 = 30 fps and 0 = no vsync
        Application.targetFrameRate = 60;
    }
}
