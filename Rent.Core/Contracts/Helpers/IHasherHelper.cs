using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Helpers
{
    public interface IHasherHelper
    {
        public string HashPassword(string password);

        public bool VerifyHashedPassword(string hashedPassword, string password);

        public bool ByteArraysEqual(byte[] b1, byte[] b2);
    }
}
