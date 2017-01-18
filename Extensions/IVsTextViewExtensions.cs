﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using System;
    using IObjectWithSite = Microsoft.VisualStudio.OLE.Interop.IObjectWithSite;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    public static class IVsTextViewExtensions
    {
        public static IVsCodeWindow GetCodeWindow(this IVsTextView textView)
        {
            Contract.Requires<ArgumentNullException>(textView != null, "textView");

            IObjectWithSite objectWithSite = textView as IObjectWithSite;
            if (objectWithSite == null)
                return null;

            Guid riid = typeof(IOleServiceProvider).GUID;
            IntPtr ppvSite;
            objectWithSite.GetSite(ref riid, out ppvSite);
            if (ppvSite == IntPtr.Zero)
                return null;

            IOleServiceProvider oleServiceProvider = null;
            try
            {
                oleServiceProvider = Marshal.GetObjectForIUnknown(ppvSite) as IOleServiceProvider;
            }
            finally
            {
                Marshal.Release(ppvSite);
            }

            if (oleServiceProvider == null)
                return null;

            Guid guidService = typeof(SVsWindowFrame).GUID;
            riid = typeof(IVsWindowFrame).GUID;
            IntPtr ppvObject;
            if (ErrorHandler.Failed(oleServiceProvider.QueryService(ref guidService, ref riid, out ppvObject)) || ppvObject == IntPtr.Zero)
                return null;

            IVsWindowFrame frame = null;
            try
            {
                frame = Marshal.GetObjectForIUnknown(ppvObject) as IVsWindowFrame;
            }
            finally
            {
                Marshal.Release(ppvObject);
            }

            riid = typeof(IVsCodeWindow).GUID;
            if (ErrorHandler.Failed(frame.QueryViewInterface(ref riid, out ppvObject)) || ppvObject == IntPtr.Zero)
                return null;

            IVsCodeWindow codeWindow = null;
            try
            {
                codeWindow = Marshal.GetObjectForIUnknown(ppvObject) as IVsCodeWindow;
                return codeWindow;
            }
            finally
            {
                Marshal.Release(ppvObject);
            }
        }

        public static ITextBuffer GetITextBuffer(this IVsTextView vstv)
        {
            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return null;
            ITextBuffer tb = wpftv.TextBuffer;
            return tb;
        }

        public static IWpfTextView GetIWpfTextView(this IVsTextView vstv)
        {
            IWpfTextView wpftv = null;
            try
            {
                wpftv = VsTextViewCreationListener.to_wpftextview[vstv];
            }
            catch (Exception eeks) { }
            return wpftv;
        }
    }
}