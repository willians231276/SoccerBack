$(document).ready(function () {
    $("#LeagueId").change(function () {
        $("#TeamId").empty();
        $("#TeamId").append('<option value="0">[Select a team...]</option>');
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#LeagueId").val() },
            success: function (teams) {
                $.each(teams, function (i, team) {
                    $("#TeamId").append('<option value="' + team.TeamId + '">' + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve team.' + ex);
            }
        });
        return false;
    });
});