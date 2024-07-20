using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBTEST.DBContext;
using OBTEST.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class GRP_PROPOSALController : ControllerBase
{
    private readonly TodoContext _context;

    public GRP_PROPOSALController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/GRP_PROPOSAL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GRP_PROPOSAL>>> GetGRP_PROPOSALS()
    {
        return await _context.GRP_PROPOSALS.ToListAsync();
    }

    // GET: api/GRP_PROPOSAL/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GRP_PROPOSAL>> GetGRP_PROPOSAL(string id)
    {
        var grp_proposal = await _context.GRP_PROPOSALS.FindAsync(id);

        if (grp_proposal == null)
        {
            return NotFound();
        }

        return grp_proposal;
    }

    //// GET: api/GRP_PROPOSAL/5
    //[HttpGet("{id}")]
    //public async Task<ActionResult<GRP_PROPOSAL>> GetGRP_PROPOSAL(string id)
    //{
    //    var grp_proposal = await _context.GRP_PROPOSALS
    //        .Where(p => p.UNID == id)
    //        .Select(p => new GRP_PROPOSAL
    //        {
    //            UNID = p.UNID,
    //            PLAN_NO = p.PLAN_NO,
    //            SERIAL_NO = p.SERIAL_NO,
    //            PLAN_CNAME = p.PLAN_CNAME,
    //            PLAN_ENAME = p.PLAN_ENAME,
    //            PLAN_YEAR = p.PLAN_YEAR,
    //            GRANTS_AMT = p.GRANTS_AMT,
    //            OWN_AMT = p.OWN_AMT,
    //            BUDGET = p.BUDGET,
    //            ACTUAL_PAY = p.ACTUAL_PAY,
    //            NATURE = p.NATURE,
    //            PLAN_TYPE = p.PLAN_TYPE,
    //            ITEM_TYPE = p.ITEM_TYPE,
    //            LY_PLAN_NO = p.LY_PLAN_NO,
    //            APPLY_ORG = p.APPLY_ORG,
    //            SPONSOR = p.SPONSOR,
    //            CONTACT_MAN = p.CONTACT_MAN,
    //            JOB_TITLE = p.JOB_TITLE,
    //            TEL = p.TEL,
    //            FAX = p.FAX,
    //            EMAIL = p.EMAIL,
    //            FULL_EXEC_SDATE = p.FULL_EXEC_SDATE,
    //            FULL_EXEC_EDATE = p.FULL_EXEC_EDATE,
    //            EXEC_SDATE = p.EXEC_SDATE,
    //            EXEC_EDATE = p.EXEC_EDATE,
    //            ACCM_RESULT = p.ACCM_RESULT,
    //            SOLVE_PROB = p.SOLVE_PROB,
    //            TOTAL_TARGET = p.TOTAL_TARGET,
    //            CURR_TARGET = p.CURR_TARGET,
    //            PROC_STEP = p.PROC_STEP,
    //            UN_BENEFIT = p.UN_BENEFIT,
    //            X_COORD = p.X_COORD,
    //            Y_COORD = p.Y_COORD,
    //            EXEC_RESULT = p.EXEC_RESULT,
    //            ENG_NO = p.ENG_NO,
    //            APPR_DOC_NO = p.APPR_DOC_NO,
    //            APPR_DATE = p.APPR_DATE,
    //            APPR_ORG = p.APPR_ORG,
    //            APPLY_MAN = p.APPLY_MAN,
    //            PLAN_ORIGIN = p.PLAN_ORIGIN,
    //            REASON_RETURN = p.REASON_RETURN,
    //            CONTINUES_MASTER_PLAN = p.CONTINUES_MASTER_PLAN,
    //            SUMMARY_FLAG = p.SUMMARY_FLAG,
    //            CLOSE_DATE = p.CLOSE_DATE,
    //            MODIFIER = p.MODIFIER,
    //            MODIFY_DATE = p.MODIFY_DATE,
    //            SUFFIX_NO = p.SUFFIX_NO,
    //            SPS_JOB_TITLE = p.SPS_JOB_TITLE,
    //            MOBILE = p.MOBILE,
    //            APPR_MAN = p.APPR_MAN,
    //            PLAN_LOCAL = p.PLAN_LOCAL,
    //            CONTACT_ORG = p.CONTACT_ORG,
    //            PLAN_TYPE_FROM = p.PLAN_TYPE_FROM,
    //            ISCONTINUE = p.ISCONTINUE,
    //            ISAUTO_NO = p.ISAUTO_NO,
    //            ACTUAL_DATE = p.ACTUAL_DATE,
    //            BUDGET_PROPERTY = p.BUDGET_PROPERTY,
    //            APPR_DOC_NO_CH = p.APPR_DOC_NO_CH,
    //            ISSINGLE_MULTIPLAN = p.ISSINGLE_MULTIPLAN,
    //            ISJOINT_APPR = p.ISJOINT_APPR,
    //            MASTER_TYPE = p.MASTER_TYPE,
    //            SINGEL_UNID = p.SINGEL_UNID,
    //            HAS_FIRST_APPR = p.HAS_FIRST_APPR,
    //            FIRST_APPR_ORG = p.FIRST_APPR_ORG,
    //            FIRST_APPR_MAN = p.FIRST_APPR_MAN,
    //            ACTUAL_YEAR = p.ACTUAL_YEAR,
    //            EXEC_SORT_FROM = p.EXEC_SORT_FROM,
    //            SEASON_APPR = p.SEASON_APPR,
    //            INTERESET_INCOME = p.INTERESET_INCOME,
    //            FORMERLY_INCOME = p.FORMERLY_INCOME,
    //            RESEARCH_INCOME = p.RESEARCH_INCOME,
    //            OTHER_INCOME = p.OTHER_INCOME,
    //            SUBSIDY_OVERSPEND = p.SUBSIDY_OVERSPEND,
    //            PRINT_SHOWDETAIL = p.PRINT_SHOWDETAIL,
    //            AXIS = p.AXIS,
    //            INNO_DESC = p.INNO_DESC,
    //            BUDGET_DESC = p.BUDGET_DESC
    //        })
    //        .SingleOrDefaultAsync();

    //    if (grp_proposal == null)
    //    {
    //        return NotFound();
    //    }

    //    return grp_proposal;
    //}


  
    private bool GRP_PROPOSALExists(string id)
    {
        return _context.GRP_PROPOSALS.Any(e => e.UNID == id);
    }
}
