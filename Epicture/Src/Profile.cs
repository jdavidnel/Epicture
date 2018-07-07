using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicture.Src
{
    class Profile
    {
        private String _username;
        private String _picture;

        public Profile(String username, String picture)
        {
            _username = username;
            _picture = picture;
        }

        public String username
        {
            get { return _username; }
            set { _username = value; }
        }

        public String picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

    }
}
