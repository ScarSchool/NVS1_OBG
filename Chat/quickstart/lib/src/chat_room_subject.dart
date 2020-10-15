// lib/src/chat_room_subject.dart
import 'dart:html';

import 'package:quickstart/src/helpers.dart';

class ChatRoomSubject {
  ChatRoomSubject(String username)
      : socket = WebSocket('ws://localhost:9780/ws?username=$username') {
    _initListeners();
  }
  final WebSocket socket;

  send(String data) => socket.send(data);
  close() => socket.close();

  _initListeners() {
    socket.onOpen.listen((evt) {
      print('Socket is open');
      send(encodeMessage(ActionTypes.newChat, null, null));
    });
  }
}
