using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSocketCasing : MonoBehaviour {

    public int socketNum;
    public LineRenderer cable;
    public CoreSocketCasing connectedSocket;

    public int getSocketNum()
    {
        return socketNum;
    }

    public void drawCable(CoreSocketCasing otherSocket)
    {
        if (connectedSocket != null) connectedSocket.undrawCable();
        if (otherSocket.connectedSocket != null) otherSocket.undrawCable();

        getCable();

        //Tell the sockets who they're now connected to
        connectedSocket = otherSocket;
        connectedSocket.connectedSocket = this;
        connectedSocket.cable = cable;

        //Set the positions
        cable.SetPosition(0, this.transform.position);
        cable.SetPosition(1, otherSocket.transform.position);
    }

    public void undrawCable()
    {
        if (cable != null)
        {
            cable.SetPosition(0, Vector3.zero);
            cable.SetPosition(1, Vector3.zero);
            cable = null;
        }

        if (connectedSocket != null)
        {
            connectedSocket.connectedSocket = null;
            connectedSocket = null;
        }
    }

    private void getCable()
    {
        cable = this.gameObject.GetComponent<LineRenderer>();
        if (cable != null) return;

        cable = this.gameObject.AddComponent<LineRenderer>();
        cable.startWidth = 0.02f;
        cable.endWidth = 0.02f;
        cable.sharedMaterial = Resources.Load<Material>("SpecialMaterials/CableMaterial");
    }

    public void redrawCable()
    {
        if (connectedSocket != null)
        {
            cable.SetPosition(0, this.transform.position);
            cable.SetPosition(1, connectedSocket.transform.position);
        }
    }
}
