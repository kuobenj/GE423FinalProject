CXX= arm-none-linux-gnueabi-gcc

targets = VBimageTX

all: $(targets)

netapi.o: netapi.c 
	$(CXX) -c netapi.c
	
omapl138_gpiofuncs.o: omapl138_gpiofuncs.c 
	$(CXX) -c omapl138_gpiofuncs.c

VBimageTX.o: VBimageTX.c
	$(CXX) -c VBimageTX.c	 

VBimageTX: netapi.o omapl138_gpiofuncs.o VBimageTX.o
	$(CXX) -o $@ $(CXXFLAGS) $(INCLUDES) $^ 
	#$(CXX) -o $@ $(CXXFLAGS) $(INCLUDES) $^ -static
clean:
	rm -rf *.o $(targets)
  
