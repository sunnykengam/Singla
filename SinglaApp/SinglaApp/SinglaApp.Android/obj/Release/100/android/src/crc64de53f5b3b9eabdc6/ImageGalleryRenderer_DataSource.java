package crc64de53f5b3b9eabdc6;


public class ImageGalleryRenderer_DataSource
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getCount:()I:GetGetCountHandler\n" +
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.ImageGalleryRenderer+DataSource, XLabs.Forms.Droid", ImageGalleryRenderer_DataSource.class, __md_methods);
	}


	public ImageGalleryRenderer_DataSource ()
	{
		super ();
		if (getClass () == ImageGalleryRenderer_DataSource.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.ImageGalleryRenderer+DataSource, XLabs.Forms.Droid", "", this, new java.lang.Object[] {  });
	}

	public ImageGalleryRenderer_DataSource (crc64de53f5b3b9eabdc6.ImageGalleryRenderer p0)
	{
		super ();
		if (getClass () == ImageGalleryRenderer_DataSource.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.ImageGalleryRenderer+DataSource, XLabs.Forms.Droid", "XLabs.Forms.Controls.ImageGalleryRenderer, XLabs.Forms.Droid", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native java.lang.Object n_getItem (int p0);


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();

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
