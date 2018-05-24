namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class ArrayAttribute : SchemaAttribute
    {
        private readonly Constants.Value _Typesize;

        internal ArrayAttribute(string name, Constants.Value Typesize) : base(name)
        {
            _Typesize = Typesize;
        }

        internal override byte[] Serialize(byte[] buffer, int position, int innerObjectPosition)
        {
            return FindVector(innerObjectPosition, position, buffer, _Typesize);
        }
    }
}
