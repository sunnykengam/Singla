package crc64de53f5b3b9eabdc6;


public class SeparatorDroidView
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.SeparatorDroidView, XLabs.Forms.Droid", SeparatorDroidView.class, __md_methods);
	}


	public SeparatorDroidView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SeparatorDroidView.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorDroidView, XLabs.Forms.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SeparatorDroidView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SeparatorDroidView.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorDroidView, XLabs.Forms.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SeparatorDroidView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SeparatorDroidView.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorDroidView, XLabs.Forms.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

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
