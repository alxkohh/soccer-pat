using System;
using System.Collections.Generic;
using System.Text;

//the namespace must be PAT.Lib, the class and method names can be arbitrary
namespace PAT.Lib {
    /// <summary>
    /// You can use static library in PAT model.
    /// All methods should be declared as public static.
    /// 
    /// The parameters must be of type "int", "bool", "int[]" or user defined data type
    /// The number of parameters can be 0 or many
    /// 
    /// The return type can be void, bool, int, int[] or user defined data type
    /// 
    /// The method name will be used directly in your model.
    /// e.g. call(max, 10, 2), call(dominate, 3, 2), call(amax, [1,3,5]),
    /// 
    /// Note: method names are case sensetive
    /// </summary>
    public class Match {

    	private static int teamA_penalty_box = 20;
    	private static int teamA_midfield_zone = 40;
    	private static int teamB_midfield_zone = 60;
    	private static int teamB_penalty_box = 80;
    	
		// int p to indicate which process it is    	
    	public static int updateBallLoc(int ball_loc, int p) 
    	{ 
    		if (p == 1) 
    		{
    			if (ball_loc != teamB_penalty_box) 
    			{ 
    				ball_loc = ball_loc + 20;  	// teamA increment by one zone 
    			}
    			return ball_loc;
    		}
    		else if (p == 2) 
    		{
    			if (ball_loc != teamB_penalty_box && ball_loc != teamB_midfield_zone)
    			{
					ball_loc = ball_loc + 40;	// teamA increment by two zones (long pass)
				}
				return ball_loc;
			}
			else if (p == 3) 
			{
				if (ball_loc != teamA_penalty_box) 
				{
					ball_loc = ball_loc - 20;	// teamB "increment" by one zone
				}
				return ball_loc;
			}
			else if (p == 4) 
			{
				if (ball_loc != teamA_penalty_box && ball_loc != teamA_midfield_zone) 
				{
					ball_loc = ball_loc - 40;	// teamB "increment" by two zones
				}
				return ball_loc;
			} else {
				throw new InvalidOperationException("Process does not exists ");
			}
    	}
    }        
}
