using System;
namespace TaxiBookingSystemEntities
{
    /// <summary>
    /// DO NOT TREAT THIS AS COMPLETE FULL FLEDGED CLASSES WITH ALL ATTRIBUTES.
    /// </summary>
    //There can whole lot of attributes associated to taxi and Car object, for sake of simplicity and time constraint I have kept it simple,
    //yet I want to demostrate the flow of my ideas and what I would write if I have to do it in object oriented fashion.
    public enum CarType
    {
        Hasback,
        Sedan,
        SUV
    }

    public abstract class Car
    {
        public int ModelId { get; set; }

        public CarType carType { get; }

    }

}
