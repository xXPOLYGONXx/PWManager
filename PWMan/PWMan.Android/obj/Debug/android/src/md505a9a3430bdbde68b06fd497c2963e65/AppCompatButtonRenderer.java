package md505a9a3430bdbde68b06fd497c2963e65;


public class AppCompatButtonRenderer
	extends md5270abb39e60627f0f200893b490a1ade.ButtonRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_dispatchTouchEvent:(Landroid/view/MotionEvent;)Z:GetDispatchTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("MR.Gestures.Android.Renderers.AppCompatButtonRenderer, MR.Gestures.Android, Version=1.5.3.0, Culture=neutral, PublicKeyToken=null", AppCompatButtonRenderer.class, __md_methods);
	}


	public AppCompatButtonRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == AppCompatButtonRenderer.class)
			mono.android.TypeManager.Activate ("MR.Gestures.Android.Renderers.AppCompatButtonRenderer, MR.Gestures.Android, Version=1.5.3.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public AppCompatButtonRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == AppCompatButtonRenderer.class)
			mono.android.TypeManager.Activate ("MR.Gestures.Android.Renderers.AppCompatButtonRenderer, MR.Gestures.Android, Version=1.5.3.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public AppCompatButtonRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == AppCompatButtonRenderer.class)
			mono.android.TypeManager.Activate ("MR.Gestures.Android.Renderers.AppCompatButtonRenderer, MR.Gestures.Android, Version=1.5.3.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public boolean dispatchTouchEvent (android.view.MotionEvent p0)
	{
		return n_dispatchTouchEvent (p0);
	}

	private native boolean n_dispatchTouchEvent (android.view.MotionEvent p0);

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
