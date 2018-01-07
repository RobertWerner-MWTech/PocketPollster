using System;
using System.Collections;
using System.Diagnostics;



namespace RefData
{

  public class RefData
  {

    // This instantiation makes the Reference data global [to every module that references Reference.dll)
    public static RefData Data = new RefData();


    public RefData()
    {
    }


//    Countries		
//      Country	
//      Name
//        DivisionType
//        PostalType
//          StatesProvs					
//          StateProv				
//            Name			
//            Communities			
//              Community		
//              Name	
//                AreaCodes	
//                AreaCode
//                  PostalCodes	
//                  PostalCode
//
//							
//                    MobileDevices							
//                    MobileDeviceType						
//                      ID					
//                      Name					
//                        Screen					
//                        Width	
//                          Height	
//                          Picture		











//    // To be used for insertion into the MobileDevices collection.
//    public class MobileDevice
//    {
//      public ushort ID;
//      public string Name;
//      public string Filename;  // Refers to a graphic image of the mobile device
//    }
//
//    public ArrayList MobileDevices = new ArrayList();


  }
}
