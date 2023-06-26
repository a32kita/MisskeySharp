using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisskeySharp
{
    [Flags]
    public enum MisskeyPermissions : UInt64
    {
        Zero                 = 0b00000000000000000000000000000000,

        Read_account         = 0b00000000000000000000000000000001,
        Write_account        = 0b00000000000000000000000000000010,

        Read_blocks          = 0b00000000000000000000000000000100,
        Write_blocks         = 0b00000000000000000000000000001000,

        Read_drive           = 0b00000000000000000000000000010000,
        Write_drive          = 0b00000000000000000000000000100000,

        Read_messaging       = 0b00000000000000000000000001000000,
        Write_messaging      = 0b00000000000000000000000010000000,

        Read_mutes           = 0b00000000000000000000000100000000,
        Write_mutes          = 0b00000000000000000000001000000000,

        // Read_notes        = 0b00000000000000000000010000000000,
        Write_notes          = 0b00000000000000000000100000000000,

        Read_notifications   = 0b00000000000000000001000000000000,
        Write_notifications  = 0b00000000000000000010000000000000,

        // Read_reactions    = 0b00000000000000000100000000000000,
        Write_reactions      = 0b00000000000000001000000000000000,

        // Read_votes        = 0b00000000000000010000000000000000,
        Write_votes          = 0b00000000000000100000000000000000,

        Read_pages           = 0b00000000000001000000000000000000,
        Write_pages          = 0b00000000000010000000000000000000,

        Read_page__likes     = 0b00000000000100000000000000000000,
        Write_page__likes    = 0b00000000001000000000000000000000,

        Read_gallary__likes  = 0b00000000010000000000000000000000,
        Write_gallary__likes = 0b00000000100000000000000000000000,
    }

    public static class MisskeyPermissionsExtensions
    {
        private static MisskeyPermissions[] s_miskkeyPermissionValues;

        static MisskeyPermissionsExtensions()
        {
            s_miskkeyPermissionValues = Enum.GetValues(typeof(MisskeyPermissions)).Cast<MisskeyPermissions>().ToArray();
        }

        public static MisskeyPermissions[] GetAllValues()
        {
            return s_miskkeyPermissionValues;
        }
    }
}
