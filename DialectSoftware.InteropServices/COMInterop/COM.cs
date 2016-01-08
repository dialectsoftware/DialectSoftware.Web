using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;

// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.InteropServices
{
    public partial class COM
    {
        [Guid("3050f669-98b5-11cf-bb82-00aa00bdce0b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        [ComImport]
        public interface IHTMLElementRender
        {
            void DrawToDC([In] IntPtr hDC);

            void SetDocumentPrinter([In, MarshalAs(UnmanagedType.BStr)] string bstrPrinterName, [In] IntPtr hDC);
        };

        [ComVisible(true), Guid("79eac9c9-baf9-11ce-8c82-00aa004ba90b"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistMoniker
        {
            void GetClassID([In, Out] ref Guid pClassID);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsDirty();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Load([In] int fFullyAvailable, [In,
                MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
                [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
                [In] int grfMode);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Save([In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
                [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
                [In] int fRemember);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SaveCompleted([In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
                [In, MarshalAs(UnmanagedType.Interface)] Object pbc);
            [return: MarshalAs(UnmanagedType.Interface)]
            IMoniker GetCurMoniker();
        }

        [ComVisible(true), ComImport(), Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistStreamInit
            {
                void GetClassID([In, Out] ref Guid pClassID);
                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int IsDirty();
                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int Load([In] IStream pstm);
                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int Save([In] IStream pstm, [In,
                    MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
                void GetSizeMax([Out] long pcbSize);
                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int InitNew();
            }

    }
}
