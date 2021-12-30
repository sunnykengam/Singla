package crc64fda671d72be6190a;


public class FileHelper
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SinglaApp.Droid.FileHelper, SinglaApp.Android", FileHelper.class, __md_methods);
	}


	public FileHelper ()
	{
		super ();
		if (getClass () == FileHelper.class)
			mono.android.TypeManager.Activate ("SinglaApp.Droid.FileHelper, SinglaApp.Android", "", this, new java.lang.Object[] {  });
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
