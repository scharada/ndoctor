import os;
import string;
import shutil;
import sys;

#---
# Deletes the file with "suo" as extension from the specified
# path starting point
#---
def CleanFiles(cleaningStartingPoint):
	for dir in os.walk(cleaningStartingPoint):
		for file in dir[2]:
			if file.endswith(".suo") or file.endswith(".user"):
				filePath = dir[0] + "\\" + file;
				print("removing " + filePath + "...");
				os.remove(filePath);

				
#---
# Detetes the 'bin' and 'obj' directories from the specified path
# starting point
#---
def CleanDirectories(cleaningStartingPoint):
		for dir in os.walk(cleaningStartingPoint):
			strDir = str(dir[0])			
			if strDir.endswith("bin") or strDir.endswith("obj") or strDir == "Release" or strDir == "Debug":
				print("removing " + strDir + "...")
				shutil.rmtree(strDir);
				
def CleanSolution(cleaningStartingPoint):		
	print("Starting point '", cleaningStartingPoint, "'")
	try:	
		print("Cleaning solution from 'bin', 'obj', 'Debug' and 'Release' directories")
		print("-----")		
		CleanDirectories(cleaningStartingPoint);
		print("")
		
		print("Cleaning solution from '.suo' and '.user' files.")
		print("-----")		
		CleanFiles(cleaningStartingPoint);
		print("")
		print("Solution cleaned!")
	except WindowsError:
			print("An error occured:", sys.exc_info()[1])
	#input("Press any key to continue...")

#---
#Starting point of the script
#---

                
if __name__ == "__main__":
	args = sys.argv;
	if len(args) != 2:
		command = "";
		for arg in args:
			command = command + " " + arg;
		raise Exception("The command is badly written! Argument expected: starting point of cleaning", command);
		
	CleanSolution(str(args[1]));
