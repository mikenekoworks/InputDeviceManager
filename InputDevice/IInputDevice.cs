using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input.Device {

    public interface IInputDevice {
        void Create( int assign_id );
        void Update();
    }

}
