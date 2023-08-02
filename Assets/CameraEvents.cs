//using UnityEngine;
//using Game.Framework;

//public class CameraEvents: GameBehaviour
//{
//    public CameraSystem cSys;

//    protected override void Setup()
//    {
//        cSys = Resolver.Resolve(typeof(CameraSystem)) as CameraSystem;
//        cSys.OnTest += HandleTest;

//        base.Setup();
//    }

//    protected override void Cleanup()
//    {
//        cSys.OnTest -= HandleTest;

//        base.Cleanup();
//    }

//    private void HandleTest()
//    {
//        Debug.Log("RECV: Test");
//    }
//}
