package crc649fee7adb2c80b102;


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
		mono.android.Runtime.register ("Checking.Droid.FileHelper, Checking.Android", FileHelper.class, __md_methods);
	}


	public FileHelper ()
	{
		super ();
		if (getClass () == FileHelper.class)
			mono.android.TypeManager.Activate ("Checking.Droid.FileHelper, Checking.Android", "", this, new java.lang.Object[] {  });
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
