function load_all_lua_file()
    require('buff_callback')
    require('timeline')
end

function debug_log(a, go, c)
    c = c + 1
    CS.UnityEngine.Debug.Log("debug_logdebug_logdebug_log")
    return c
end

load_all_lua_file()