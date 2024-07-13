# CacheCoherencyProtocols
CacheCoherencyProtocols demonstration base on Dividni(https://dividni.com)

auto generate assignment for student to understand cache coherency protocals. 

The assignments are divided into topics based on different protocols. Task1 is for MSI, task2 is for MESI, task3 is for MOSI, and task4 is for MOESI. They are not divided according to the knowledge points examined. Each task examines the status changes and corresponding status of the protocol. Performance operations (I also consolidated them myself).

Let students understand the state changes under the same operation under different protocols when a multi-processor based on 4 cores performs read and write operations on the same address, so that students can better understand the rules of state changes.

But in addition to status changes, I also added additional statistics on the number of memory wire-back and invalid operations to allow students to understand the performance differences under different protocols. However, considering the workload of actual students answering questions, I only randomly generated 5 operations (of course this can be adjusted).

The consequence of this is that if you need to evaluate the performance comparison of protocols based on the results of these operations, you may need to customize specific read and write operation styles to simulate special or extreme read and write environments to highlight the comparison, but I personally feel that if Students can correctly answer the status changes under the four protocols, as well as the number of memory wire-back and invalid operations, and can fully understand the entire content of the cache coherence protocol. Moreover, using specific modes of reading and writing operations to set questions cannot actually fully detect students’ all state change rules under different protocols.

So although only five random operations may produce differences in the difficulty of students' answering questions, and the comparison of the advantages and disadvantages between protocols is not obvious, I hope you can understand the reason for my design.
