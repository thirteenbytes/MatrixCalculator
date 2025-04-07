# InvestCloud .Net Technical Interview Question  

A service exists that provides numerical data from a pair of two-dimensional datasets A and B. The 
contents and dimensions of A and B can be interpreted as two 2D square matrices, which when 
multiplied together produce a third matrix that is the desired result of this coding test. 
Write a program that retrieves the datasets A & B, multiplies their matrix representations (A X B), and 
submits the result back to the service. 

1. The service API description at https://recruitment-test.investcloud.com/. 
2. Initialize the dataset size to 1000 x 1000 elements. Doesn't count towards total runtime. 
3. The result matrix must be formatted as a concatenated string of the matrix' contents (left-to
right, top-to-bottom), hashed using the md5 algorithm. Submit the md5 hash to validate your 
result and receive a passphrase from the service indicating success or failure. 
4. Total runtime should be as fast as possible, given the size of the datasets, the nature of the 
service API, and the mathematical operation requested (cross product of 2 matrices) 
5. For your reference, we have an implementation that completes with a runtime of data 
retrieval and computation in around 30 seconds, understood this depends on various factors. 

## Reference for Matrix Multiplication (A X B):
![image](https://github.com/user-attachments/assets/c6855178-7879-470c-a843-ed6e26c06f23)

1. From Visual Studio - Clone a repository
2. Set InvestCloud.ConsoleUI as Start up project
3. You can change size of the Matrices you wan to calcuate in the Program.cs. await svc.Run(2). We tested 1000 to test how fast it is.

Expected out:
[12:15:44 INF] Application starting:
[12:15:44 INF] Step 1: Initializing and build the square matrices (1000 x 1000)
[12:16:11 INF] Step 2: Multiple the two matrices
[12:16:20 INF] Step 3: Create the MD5 Hash for validation.
[12:16:20 INF] Step 4: Validating the MD5 Hash 1952421063211517718112143121254201247179132186
[12:16:20 INF] Step 5: Passphrase: Kraken Approves!

Note this accesses the company API to get the data.
