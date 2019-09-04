using SharpDX;

namespace Glue
{
    public class CameraStruct
    {
        public Vector3 Position;
        public Vector4 Rotation;
        public Matrix Projection;
        public ushort ResolutionX;
        public ushort ResolutionY;

        public override string ToString()
        {
            return $"pos: {Position.ToString()}, rot: {Rotation.ToString()}";
        }

        public CameraStruct()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Vector4(0, 0, 0, 1);
            Projection = new Matrix();
            ResolutionX = 1920;
            ResolutionY = 1080;
        }

        public CameraStruct(
            Vector3 position,
            Vector4 rotation,
            Matrix projection,
            ushort resolutionX,
            ushort resolutionY)
        {
            Position = position;
            Rotation = rotation;
            Projection = projection;
            ResolutionX = resolutionX;
            ResolutionY = resolutionY;
        }
    }

    public class CameraTextureTargetStruct
    {
        public uint ColorHandle;
        public uint DepthHandle;

        public CameraTextureTargetStruct() { }

        public CameraTextureTargetStruct(uint colorHandle, uint depthHandle)
        {
            ColorHandle = colorHandle;
            DepthHandle = depthHandle;
        }
    }
}
