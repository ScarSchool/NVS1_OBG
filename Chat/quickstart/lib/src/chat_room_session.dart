import 'dart:io';
import 'dart:convert';

import 'package:quickstart/src/helpers.dart';

class Chatter {
  Chatter({this.session, this.socket, this.name});
  HttpSession session;
  WebSocket socket;
  String name;
}

class ChatRoomSession {
  final List<Chatter> _chatters = [];
  addChatter(HttpRequest request, String username) async {
    WebSocket ws = await WebSocketTransformer.upgrade(request);
    Chatter chatter = Chatter(
      session: request.session,
      socket: ws,
      name: username,
    );
    // Listen for incoming messages, handle errors and close events
    chatter.socket.listen(
      (data) => _handleMessage(chatter, data),
      onError: (err) => print('Error with socket ${err.message}'),
      onDone: () => _removeChatter(chatter),
    );
    _chatters.add(chatter);
    print('[ADDED CHATTER]: ${chatter.name}');
  }

// Update _handleMessage implementation
  _handleMessage(Chatter chatter, data) {
    print('[INCOMING MESSAGE]: $data');
    // Decode and check action types of payload
    Map<String, dynamic> decoded = json.decode(data);
    var actionType = getActionType(decoded['type']);
    var message = decoded['data'];
    // Reducer logic to handle different chat action types
    switch (actionType) {
      case ActionTypes.newChat:
        // Welcome connected participant
        chatter.socket.add(encodeMessage(
          ActionTypes.newChat,
          null,
          'Welcome to the chat ${chatter.name}',
        ));
        // Tell the others of this new participant
        _notifyChatters(
          ActionTypes.newChat,
          chatter,
          '${chatter.name} has joined the chat.',
        );
        break;
      case ActionTypes.chatMessage:
        // Display the message on the participant's screen using 'You' instead of their name
        chatter.socket.add(encodeMessage(
          ActionTypes.chatMessage,
          'You',
          message,
        ));
        // Broadcast the chat message to the other participants using the participant's username
        _notifyChatters(ActionTypes.chatMessage, chatter, message);
        break;
      case ActionTypes.leaveChat:
        // Cancel the socket when a participant leaves.
        // This will trigger the onDone event we defined earlier, removing its associated `Chatter` object from _chatters
        chatter.socket.close();
        break;
      default:
        break;
    }
  }

  _notifyChatters(ActionTypes actionType, Chatter exclude, [String message]) {
    // Set the from value dependent on the action type. When set to null, its treated as a generic message from the chat server
    var from =
        actionType == ActionTypes.newChat || actionType == ActionTypes.leaveChat
            ? null
            : exclude.name;
    _chatters
        .where((chatter) => chatter.name != exclude.name)
        .toList()
        .forEach((chatter) => chatter.socket.add(encodeMessage(
              actionType,
              from,
              message,
            )));
  }

  _removeChatter(Chatter chatter) {
    print('[REMOVING CHATTER]: ${chatter.name}');
    _chatters.removeWhere((c) => c.name == chatter.name);
    _notifyChatters(
      ActionTypes.leaveChat,
      chatter,
      '${chatter.name} has left the chat.',
    );
  }
}
