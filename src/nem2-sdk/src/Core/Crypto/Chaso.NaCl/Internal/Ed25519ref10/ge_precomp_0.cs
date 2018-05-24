namespace io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10
{
	internal static partial class GroupOperations
	{
		internal static void ge_precomp_0(out GroupElementPreComp h)
		{
			FieldOperations.fe_1(out h.yplusx);
			FieldOperations.fe_1(out h.yminusx);
			FieldOperations.fe_0(out h.xy2d);
		}
	}
}