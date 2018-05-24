namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class ScalarAttribute : SchemaAttribute
    {
        private readonly Constants.Value _size;

        internal ScalarAttribute(string name, Constants.Value Typesize) : base(name)
        {       
            _size = Typesize;
        }

        internal override byte[] Serialize(byte[] buffer, int position, int innerObjectPosition)
        {
            return FindParam(innerObjectPosition, position, buffer, _size);
        }
    }
}
