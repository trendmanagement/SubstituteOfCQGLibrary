# README #

This is a repository with CQG library substitute system that allows one to use single CQG account from multiple applications working at the same time. There is one server application named "data collector" that connects to CQG account and shares its services with one or more client applications named "realtimes". Data collector communicates with realtimes through MongoDB.

### PREREQUISITES ###

1. CQG Integrated Client must be installed on the machine where data collector runs. It's necessary to download and install file "16x14880_NET_b1.exe" from [this](ftp://ftp.cqg.com/CQGIC/) FTP server.

2. MongoDB Server must be installed and set up on the machine where data collector runs. You can download and install the community version from [this](https://www.mongodb.com/download-center?jmp=nav#community) site. After the installation, you need to configure a windows service for MongoDB according to [these](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/#configure-a-windows-service-for-mongodb-community-edition) instructions.

3. Make sure that data collector and all realtimes use the same URL for MongoDB.

Notice that a realtime does not require either installation of CQG Integrated Client or MongoDB Server on the machine where it runs. All the staff it needs is already included into the fake CQG library which is referenced from the realtime.

MongoDB content is also nothing to think about. Data collector and fake CQG library used by realtimes handle it for you.

### SIMILARITY ###

1. Fake CQG library (FL) provides API being almost a mirror copy of real CQG library (RL) API. That means, it contains similar interfaces, classes, structs, enums, delegates, events, constructors, destructors, methods, properties and fields. In most cases, you can work with FL objects in the same way as you did for RL objects. At the same time, FL does not have a reference to RL assembly "Interop.CQG.dll". (Only data collector has a reference to it.)

2. Realtime does not require any specific steps to establish connection between it and data collector. Just start working with FL objects as if you worked for RL objects. FL does lazy connection to MongoDB on the first operation, e.g. creation of fake CQGCEL object.

3. Exceptions are delivered from data collector to realtimes. That means, if you do some operation in realtime code that leads to exception on data collector side, you will be able to catch, analyze and process this exception in realtime. Data collector is invulnerable to these exceptions and does not stop working after them.

### DIFFERENCES ###

1. The root namespace of FL is named "FakeCQG" while the root namespace of RL is named "CQG". If you still want to use "CQG" name in your program, create a namespace alias as following:

    ```
        using CQG = FakeCQG;
    ```

2. FL is not a COM assembly, therefore, you cannot instantiate interfaces contained in it. Instantiation of RL interface named "SomeName" should be replaced with instantiation of FL class named "SomeName*Class*". All constructor arguments will be the same.

3. FL provides some extra APIs that are not present in RL. This staff maintains working of FL with MongoDB and other internal operations. The extra APIs are encapsulated into FakeCQG.Internal namespace. It's unlikely that you will need to use them while developing new realtime with FL.

4. FL objects are not compatible with RL objects. Therefore, do not try to use both FL and RL in one project and, for example, pass FL objects to RL methods or vice versa.

### PERFORMANCE ###

Performance of FL is worse than performance of RL.

1. There is a time delay between an operation on FL object and the operation on corresponding RL object. The delay is implied by passing of some data from realtime to data collector and in opposite direction though MongoDB for each operation. Network health matters.

2. Both realtime and data collector use several timers for different purposes. As a result, we get a reaction on command or event with a delay depending on timers periods. One timer is used in data collector for looking for queries from realtimes, other timer is used in realtime for looking for answers from data collector. There are some other timers. Their periods are subjects of adjustment for given environment and hardware.

3. Data collector can work with several realtimes at the same time which will cause additional delays for a given realtime due to race condition.

### RESTRICTIONS ###

When you develop new realtime with FL, keep in mind that operations you perform on FL objects are projected into RL objects, but this projection is not straightforward in some cases.

1. Data collector maintains persistent connection to CQG service and no one realtime is allowed to drop it. When a realtime calls methods CQG.Startup and CQG.Shutdown, data collector just does nothing. The property CQG.IsStarted equals True even before call of the first method and after call of the second method.

2. As long as RL connects to CQG service before realtime calls method CQG.Startup, realtime is not allowed to assign some properties. For example, the next three properties cannot be assigned for FL:
 
    * CEL.APIConfiguration.ReadyStatusCheck
    * CEL.APIConfiguration.CollectionsThrowException
    * CEL.APIConfiguration.TimeZoneCode

    An attempt to assign any of them from a realtime leads to the next exception:

    *CQGAPIConfig: Failed to set the value of property 'ReadyStatusCheck'. CQG API is running. The parameter's value cannot be changed on the fly.*

### DEVELOPERS ###

Please don't hesitate to contact us by the matters of this software:

[saleksin@unboltsoft.com](mailto:saleksin@unboltsoft.com)

[vzagubinoga@unboltsoft.com](mailto:vzagubinoga@unboltsoft.com)

[rlachinov@unboltsoft.com](mailto:rlachinov@unboltsoft.com)