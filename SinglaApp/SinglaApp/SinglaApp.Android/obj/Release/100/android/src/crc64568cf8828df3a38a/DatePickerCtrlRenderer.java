package crc64568cf8828df3a38a;


public class DatePickerCtrlRenderer
	extends crc643f46942d9dd1fff9.DatePickerRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SinglaApp.Droid.Renders.DatePickerCtrlRenderer, SinglaApp.Android", DatePickerCtrlRenderer.class, __md_methods);
	}


	public DatePickerCtrlRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == DatePickerCtrlRenderer.class)
			mono.android.TypeManager.Activate ("SinglaApp.Droid.Renders.DatePickerCtrlRenderer, SinglaApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public DatePickerCtrlRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == DatePickerCtrlRenderer.class)
			mono.android.TypeManager.Activate ("SinglaApp.Droid.Renders.DatePickerCtrlRenderer, SinglaApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public DatePickerCtrlRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DatePickerCtrlRenderer.class)
			mono.android.TypeManager.Activate ("SinglaApp.Droid.Renders.DatePickerCtrlRenderer, SinglaApp.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
