using System;
using Newtonsoft.Json;
using SocketIOClient;

public class RoomData
{
    [JsonProperty("roomId")]
    public string roomId { get; set; }
}

public class UserData
{
    [JsonProperty("usedId")]
    public string userId { get; set; }   
}

public class MoveData
{
    [JsonProperty("position")]
    public int position { get; set; }
}

public enum MultiplayManagerState
{
    CreateRoom,     // 방 생성
    JoinRoom,       // 방 참가
    StartGame,      // 두 유저가 방에 모두 들어와서 게임을 시작
    ExitRoom,       // 자신이 방 빠져 나갔을 때
    EndGame         // 상대방 접속 끊거나 방을 나갔을 때
}

public class MultiplayManager : IDisposable
{
    private SocketIOUnity _socket;
    private event Action<MultiplayManagerState, string> _onMultiplayStateChanged;
    public Action OnReceiveMessage;
    public Action<MoveData> OnOpponentMove;

    public MultiplayManager(Action<MultiplayManagerState, string> onMultiplayStateChanged)
    {
        _onMultiplayStateChanged = onMultiplayStateChanged;

        var uri = new Uri(Constants.SocketURL);
        _socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket    
        });

        _socket.On("createRoom", CreateRoom);
        _socket.On("createRoom", JoinRoom);
        _socket.On("createRoom", StartGame);
        _socket.On("createRoom", ExitRoom);
        _socket.On("createRoom", EndGame);
        _socket.On("doOpponent", DoOpponent);

    }

    // 클라이언트가 서버에 접속 -> 방 새로 생성 -> 서버가 호출해 주는 함수
    private void CreateRoom(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(MultiplayManagerState.CreateRoom, data.roomId);
    }

    // 클라이언트가 서버에 접속 -> 대기중인 방이 있음 -> 방에 참가 했을때 서버가 호출해 주는 함수
    private void JoinRoom(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(MultiplayManagerState.JoinRoom, data.roomId);
    }

    // 방에 참가한 유저가 게임을 시작할 때 서버가 호출해 주는 함수
    private void StartGame(SocketIOResponse response)
    {
         var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(MultiplayManagerState.StartGame, data.roomId);       
    }

    // 방에 참가한 유저가 방을 나갔을 때 서버가 호출해 주는 함수
    private void ExitRoom(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(MultiplayManagerState.ExitRoom, null);      
    }

    // 방에 참가한 유저가 접속을 끊었을 때 서버가 호출해 주는 함수
    private void EndGame(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(MultiplayManagerState.EndGame, null);  
    }

    // 상대방이 게임을 진행 했을때 서버가 호출해 주는 함수
    private void DoOpponent(SocketIOResponse response)
    {
        var data = response.GetValue<MoveData>();
        OnOpponentMove?.Invoke(data);  
    }

    // 플레이어가 마커를 놓았을 때 서버로 이동 정보 전송
    public void SendPlayerMove(string roomId, int position)
    {
        _socket.Emit("doPlayer", new { roomId, position });    
    }

    public void LeaveRoom(string roomId)
    {
        _socket.Emit("leaveRoom", new { roomId });
    }

    public void Dispose()
    {
        if(_socket != null)
        {
            _socket.Disconnect();
            _socket.Dispose();
        }
    }
}
