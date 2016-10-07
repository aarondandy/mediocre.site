Title: "A Simple Readable C# Micro-optimization: Expression Operand Ordering"
Published: 9/2/2013
Tags:
  - C#
  - JIT
---

TL;DR: Try moving the most nested portions of your expressions to the left so that your expression as much as possible executes from left to right.

Programmers acquire habits as they progress in their craft.
Some are learned from books and teachers such as the 'i' variable in a for loop.
Others such as [yoda conditions](http://www.codinghorror.com/blog/2012/07/new-programming-jargon.html) are learned from blowing a toe off.
Even in C# where an accidental assignment in a condition often is a compile time error I still have trouble breaking the habit of using yoda conditions.

I have caught myself forming a new habit lately.
When I create and especially when I refactor large or complex expressions I now find myself bubbling the deepest nested parentheses as far left as possible.
This increases my understanding of the expression but your mileage may vary.
Recently when refactoring a large expression in this fashion I also happened to be timing its execution.
I found that the more I moved the deepest portions of the expression to the left the better the performance.
While the performance gains are minor I do see a trend towards better overall performance.

## A Small Example

Lets look at the following quick toy examples and my interpretation of them.

Multiply 'b' by 'c' then add 'a' to the product.

```csharp
static int LeftHeavy(int a, int b, int c) {
    return ( b * c ) + a;
}
```

Remember 'a' then multiply 'b' by 'c' then add the product to the first value.

```csharp
static int RightHeavy(int a, int b, int c) {
    return a + ( b * c );
}
```

Both methods produce the same result but to me one is easier on the brain.
On a good day it seems as if my brain has a stack with a maximum depth of two so when something is confusing I tend to write my code with that in mind, just like the `LeftHeavy` example.
When I read it literally the `RightHeavy` example requires me to perform an extra step using my memory so I can later use the 'a' variable.

Likewise, the CIL shows the same.

The `LeftHeavy` example compiled to CIL:

```
ldarg.1
ldarg.2
mul
ldarg.0
add
ret
```

The `RightHeavy` example compiled to CIL:

```
ldarg.0
ldarg.1
ldarg.2
mul
add
ret
```

The generated CIL instructions are very similar.
Both methods generate the same number of each instruction but the order is slightly different.
In the `RightHeavy` example there are three consecutive loads and as a result the method requires 3 values on the stack at one time while the other requires only 2.
Next lets look at the differences in the generated x86 instructions.

The `LeftHeavy` x86 instructions:

```
mov         ebp,esp
mov         eax,dword ptr [ebp+8]
imul        eax,edx
add         eax,ecx
```

The `RightHeavy` x86 instructions:

```
mov         ebp,esp
mov         eax,ecx
mov         ecx,edx
mov         edx,dword ptr [ebp+8]
imul        edx,ecx
add         eax,edx
```

You can see that the `LeftHeavy` method has less x86 instructions and spends less time shuffling things around.
It seems as if the C# compiler and JIT compiler do exactly as you say and not much more.
This makes some sense as you wouldn't want the compilers making any false assumptions about your logic or side effects.

The following example would certainly not work correctly if it was reordered arbitrarily and has side effects:

```csharp
var result = ( Step2() * Step3() ) + Step1();
```
## A Larger Example

Lets look at a slightly more realistic example, a Cassini-Soldner projection (my attempt at it).
You can find the [attempted implementation](https://bitbucket.org/aarondandy/expressionorderingoptimization) over on BitBucket and try it yourself to [see the JIT output](https://blogs.msdn.microsoft.com/vancem/2006/02/20/how-to-use-visual-studio-to-investigate-code-generation-questions-in-managed-code/).
Similar to the previous example, the same number of CIL instructions are generated but the "Left" method has a maxstack of 4 while the "Right" has a maxstack of 9.
The resulting x86 instructions after JIT compilation also have a few differences.
The arithmetic instructions are slightly different between the two but there are the same number of total arithmetic instructions.
The major difference between the two is that the "Right" version has 5 more `fld`, 2 more `fstp`, and 2 more `fxch` instructions.

## Conclusion

While I cannot prove that this optimization works consistently I have yet to see it decrease performance.
My observations of this method have been that the number of x86 instructions is always reduced (x64 is dramatically reduced) and the performance always either is mostly equivalent or slightly better.
I cannot however say with any certainty that there does not exist a situation where performance is decreased.
Another thing to be aware of is that when re-ordering commutative operands the floating point results may differ slightly but you could argue that both results are equally incorrect and approximate.