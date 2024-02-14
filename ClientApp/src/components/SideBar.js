import React from "react";
import LOGO from "../assets/gra.png";

const SideBar = () => {
  return (
    <div className="w-0 md:w-[45%] lg:w-[35%] bg-side-bg flex flex-col place-content-center overflow-hidden">
      <div className="grid place-content-center gap-2">
        <img alt="logo" src={LOGO} width={250} className="shadow-2xl" />

        <h4 className="text-1xl text-white md:text-2xl lg:text-4xl">
          Verify your invoice
        </h4>
      </div>
      <div className="grid place-content-center mt-2">
      <p className=" text-[#00a4d7]">
          Insist on a valid invoice when you make a purchase.
        </p>
      </div>
    </div>
  );
};

export default SideBar;
