namespace io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10
{
	internal static partial class GroupOperations
	{
		internal static void ge_p2_0(out  GroupElementP2 h)
		{
			FieldOperations.fe_0(out h.X);
			FieldOperations.fe_1(out h.Y);
			FieldOperations.fe_1(out h.Z);
		}
	}
}