/*   Do *not* directly modify this file.  It was    */
/*   generated by the Configuration Tool; any  */
/*   changes risk being overwritten.                */

/* INPUT gigous_kuo_lab5.cdb */

/*  Include Header Files  */
#include <std.h>
#include <prd.h>
#include <hst.h>
#include <swi.h>
#include <tsk.h>
#include <log.h>
#include <sem.h>
#include <que.h>
#include <sts.h>

#ifdef __cplusplus
extern "C" {
#endif

extern PRD_Obj PRD_StartADC;
extern HST_Obj RTA_fromHost;
extern HST_Obj RTA_toHost;
extern SWI_Obj PRD_swi;
extern SWI_Obj KNL_swi;
extern SWI_Obj SWI_control;
extern TSK_Obj TSK_idle;
extern TSK_Obj TSK_I2C;
extern TSK_Obj TSK_atmelcompass;
extern LOG_Obj LOG_system;
extern LOG_Obj trace;
extern SEM_Obj SEM_atmel_rcservo;
extern SEM_Obj SEM_I2CSendStrmsg_rdy;
extern QUE_Obj I2CSendStrfreeQueue;
extern QUE_Obj I2CSendStrmsgQueue;
extern STS_Obj IDL_busyObj;


#ifdef __cplusplus
}
#endif /* extern "C" */
