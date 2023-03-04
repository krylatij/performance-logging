
# Description
This is a sandbox to test different approaches on how to track an particular method performance.

# Results

|           Method |     Mean |    Error |   StdDev |   Median |
|----------------- |---------:|---------:|---------:|---------:|
|      AutofacSync | 313.7 ns |  5.97 ns |  4.99 ns | 311.4 ns |
|     AutofacAsync | 377.8 ns |  4.53 ns |  3.78 ns | 377.0 ns |
|  MethodTimerSync | 294.5 ns |  5.51 ns |  4.60 ns | 294.2 ns |
| MethodTimerAsync | 464.8 ns | 26.72 ns | 78.80 ns | 446.5 ns |
|    PostsharpSync | 274.6 ns | 10.64 ns | 29.66 ns | 270.2 ns |
|   PostsharpAsync | 327.6 ns |  7.44 ns | 21.22 ns | 321.2 ns |
