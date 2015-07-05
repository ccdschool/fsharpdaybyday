# Day #15
After two days of "just theory" it's time again for some practice. Here's a new challenge for you: the [application kata "LOC Counter"](https://app.box.com/s/zwwwlyex8bvf0wq1pn9tmbzryhrla51i).

## Problem analysis
There are two main aspects to the problem:

* **Acquire** source text lines from files and folders. Folders need to searched recursively for relevant files. Only files with extension .cs are relevant. The .NET BCL provides functions which make this very easy.
* **Analyze** the source text lines, i.e. count the non-comment and non-whitespace lines. This is the heart of the program, its domain. Handling multi-line comments will probably require some form of "state machine" to track if a line is inside or outside a multi-line comment.

The source file locations (filenames and foldernames) can be **read from the command line**.

And the results are **written to the console**.

What about errors? If the **program gets started without locations** the user should be informed about the correct usage. This does not seem to be an exceptional situation.

If files are given which are not relevant or are not existing then they will be skipped silently. The specification does not demand otherwise.

Any other error situation signalled by an exception will lead to program termination. The **exception should be displayed**.

## Solution design
The aspects found during analysis make good candidates for functional units in a data flow (Figure 1).

![Figure 1](images/fig1.jpeg)

Each "bubble" is an I/O processor; it transforms input into output. As long as it's a bubble, it's a conceptual thing and part of the solution. It's not yet code. So calling it a function would be misleading.

Functions are implementation details. During solution design you want to stay nimble and not let yourself get bogged down by too much thinking about implementation and all its details.

Solution design tries to solve the problem on an abstract level, in your head. And drawing a diagram like the above just makes more explicit what's going on in your head ;-) It's a picture for a mental model.

The mental model of software solutions should not be some form of imperative, algorithmic description of what needs to be done. Then there would be hardly a difference between it and code. You'd be slow to come up with it, and it would be hard to keep it all in your head.

Rather it should be a declarative description of a process transforming some input into some output. That's what figure 1 shows: A five step process to transform command line parameters (_argv_) into output an exit code - while causing a side effect on the console (_report ..._).



### Read more
