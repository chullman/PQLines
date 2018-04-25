package md5d10f64e67bc76393920d5b91cc607975;


public class ThemedPopupButtonRenderer
	extends md5d10f64e67bc76393920d5b91cc607975.ButtonRendererWithNavFix
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PQLines.Droid.CustomRenderers.ThemedPopupButtonRenderer, PQLines.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ThemedPopupButtonRenderer.class, __md_methods);
	}


	public ThemedPopupButtonRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ThemedPopupButtonRenderer.class)
			mono.android.TypeManager.Activate ("PQLines.Droid.CustomRenderers.ThemedPopupButtonRenderer, PQLines.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ThemedPopupButtonRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ThemedPopupButtonRenderer.class)
			mono.android.TypeManager.Activate ("PQLines.Droid.CustomRenderers.ThemedPopupButtonRenderer, PQLines.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public ThemedPopupButtonRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ThemedPopupButtonRenderer.class)
			mono.android.TypeManager.Activate ("PQLines.Droid.CustomRenderers.ThemedPopupButtonRenderer, PQLines.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
