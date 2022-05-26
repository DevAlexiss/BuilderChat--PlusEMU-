using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Plus.Communication.Packets.Outgoing.Messenger;

namespace Plus.Communication.Packets.Incoming.Messenger
{
    class SendMsgEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int userId = Packet.PopInt();
            if (userId == 0 || userId == Session.GetHabbo().Id)
                return;

            string message = PlusEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Packet.PopString());
            if (string.IsNullOrWhiteSpace(message))
                return;


            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendNotification("Oops, you're currently muted - you cannot send messages.");
                return;
            }

            if (userId == 0x7fffffff)
            {

                PlusEnvironment.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(0x7fffffff, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);
                return;
            }


            Session.GetHabbo().GetMessenger().SendInstantMessage(userId, message);

        }
    }
}