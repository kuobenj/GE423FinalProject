CXX = arm-none-linux-gnueabi-gcc

targets = gigous_kuo_lab6_linux

all: $(targets)

netapi.o: netapi.c 
	$(CXX) -c netapi.c
	
omapl138_gpiofuncs.o: omapl138_gpiofuncs.c 
	$(CXX) -c omapl138_gpiofuncs.c

gigous_kuo_lab6_linux.o: gigous_kuo_lab6_linux.c
	$(CXX) -c gigous_kuo_lab6_linux.c	 

gigous_kuo_lab6_linux: netapi.o omapl138_gpiofuncs.o gigous_kuo_lab6_linux.o
	$(CXX) -o $@ $(CXXFLAGS) $(INCLUDES) $^ 
clean:
	rm -rf *.o $(targets)
  
