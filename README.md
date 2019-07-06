# Cloud Storage AWS S3 Services

## Project Requirements

This is an ASP.NET Web API project focusing on the AWS Cloud S3 APIs, nevertheless the core classes implementing IS3SliteLib can be applied to any other kinds of C# projects: Console, Windows Forms, Web Forms, WPF, ASP.NET MVC, Microservices, or even used by any other libraries working as an infrastructure component after the modest modification.

In the large cloud deployment related projects, considering efficiency and cost, the structural layered architecture is often an optimal solution.

In this web application, there exists one type of important and complex data structure, here it might as well be called MessagePacket, basically it's JSON format, every such packet contains its own unique image with the average size 500 KB. It's mandatory to persist them to some data store. Apparently, those packets and images are so-called BLOBs, need be treated differently for convenient storage and retrieval later on.

According to the technical specification, it needs store huge amount of the records mentioned above, the number of which keeps increasing every second, more importantly these records must be kept for at least 10 years, and must be always ready to be accessed.

Inside the MessagePacket, the image part which should be stored in the format of Base64 code string occupies average 97% of the total size of a single MessagePacket.

So, we need to design a special architecture for the storage mechanism, here we choose Amazon S3.

"Divide and Conquer", "Layered", yes, they are!

## Project Architecture

The strategy is to introduce the 3-layer hierarchy:

- Layer 1: The core important data of MessagePacket ought to be put into a relation database such as SQL Server, which is efficient, easy to manipulate, but very expensive. For this purpose we design a Table called tbl_mp_core. In tbl_mp_core, there are two special fields, one is called ReferenceID, another is EntryID.

- Layer 2: Two S3 buckets are employed, one is to store the MessagePackets called S3BucketMessagePackets, another is to store the corresponding images called S3BucketMPImages. 
With each ReferenceID, you can download/upload a single Sqlite3 database file which contains a Sqlite3 table tbl_message_packet. Only two fields, one is used as a "key" called EntryId, another is accordingly used as "value" called Content which is in JSON format containing a reference to a unique physical image file so-called imageID.

- Layer 3: We store the corresponding image of each MessagePacket into S3 bucket S3BucketMPImages, the imageID is used as also a refenceID, with which we can download via S3 APIs the image in the form either a disk file or a ResponseStream which finally will be converted to a base64 code string.

Please feel free to ask me if you have any questions.

Have fun,

Jerry Sun

Email:    jerysun007@hotmail.com

Website:  https://sites.google.com/site/geekssmallworld
