using System.Collections.Generic;
using UnityEngine;

public interface IChunkMover
{
    void MoveForward(List<Transform> chunks, float speed);
    void HandleLateralInput();
    void UpdateLateralPosition();
    void SetLateralLimits(float min, float max);
}
