using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RawTypeParser
{
    /// <summary>
    /// Reads a <see cref="Stream"/> or <see cref="{byte[]}"/> and attempts
    /// to parse the given <see cref="{T}"/> from the data.
    /// </summary>
    public sealed class RawTypeParser : IRawTypeParser
    {
        /// <summary>
        /// Attempts to read a struct of type <typeparamref name="T"/> from the given stream.
        /// </summary>
        /// <typeparam name="T">The type of struct to read from the given <paramref name="stream"/>.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to read from.</param>
        /// <returns>A struct of type <typeparamref name="T"/> from the given <paramref name="stream"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// When the given <paramref name="stream"/> is null or Stream.Null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When the given <paramref name="stream"/> is smaller than the size of <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The layout of T is not sequential or explicit. 
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// The class specified by T does not have an accessible parameterless constructor.
        /// </exception>
        public T ByteArrayToStructure<T>(Stream stream) where T : struct
        {
            int length = Marshal.SizeOf(typeof(T));
            var objectBytes = new byte[length];

            stream.Read(objectBytes, 0, length);
            return ByteArrayToStructure<T>(objectBytes);
        }

        /// <summary>
        /// Attempts to read a struct of type <typeparamref name="T"/> from the given byte array.
        /// </summary>
        /// <typeparam name="T">The type of struct to read from the given <paramref name="bytes"/>.</typeparam>
        /// <param name="bytes">The byte array to read from.</param>
        /// <returns>A struct of type <typeparamref name="T"/> from the given <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// When the given <paramref name="stream"/> is null or Stream.Null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When the given <paramref name="stream"/> is smaller than the size of <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The layout of T is not sequential or explicit. 
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// The class specified by T does not have an accessible parameterless constructor.
        /// </exception>
        public T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
