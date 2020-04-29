using ReGoap.Unity;

namespace GPG220.Dylan.Scripts.GOAPFirstTry
{
    public class GoapSensor : ReGoapSensor<string, object>
    {
        void FixedUpdate()
        {
            var worldState = memory.GetWorldState();
            worldState.Set("mySensorValue", 1); // like always myValue can be anything... it's a GoapState :)
        }

    }
}
